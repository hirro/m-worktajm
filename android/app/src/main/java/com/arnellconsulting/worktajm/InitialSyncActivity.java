package com.arnellconsulting.worktajm;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.Toolbar;
import android.view.View;

import com.android.volley.AuthFailureError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.RequestFuture;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.model.TimeEntry;
import com.arnellconsulting.worktajm.utils.LogService;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.HashMap;
import java.util.Map;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

public class InitialSyncActivity extends AppCompatActivity {

    /**
     * Keep track of the login task to ensure we can cancel it if requested.
     */
    private SyncTask mSyncTask = null;
    private static final String ACTIVITY_NAME = "InitialSync";
    private View mProgressView;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_initial_sync);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        mProgressView =  findViewById(R.id.projectProgressBar);

//        FloatingActionButton fab = (FloatingActionButton) findViewById(R.id.fab);
//        fab.setOnClickListener(new View.OnClickListener() {
//            @Override
//            public void onClick(View view) {
//                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
//                        .setAction("Action", null).show();
//            }
//        });

        showProgress(true);
        mSyncTask = new SyncTask(this, MySingleton.getLoginResponse().getAuthToken());
        mSyncTask.execute((Void) null);
    }


    /**
     * Shows the progress UI and hides the login form.
     */
    @TargetApi(Build.VERSION_CODES.HONEYCOMB_MR2)
    private void showProgress(final boolean show) {
        // On Honeycomb MR2 we have the ViewPropertyAnimator APIs, which allow
        // for very easy animations. If available, use these APIs to fade-in
        // the progress spinner.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2) {
            int shortAnimTime = getResources().getInteger(android.R.integer.config_shortAnimTime);
            mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
            mProgressView.animate().setDuration(shortAnimTime).alpha(
                    show ? 1 : 0).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
                }
            });
        } else {
            // The ViewPropertyAnimator APIs are not available, so simply show
            // and hide the relevant UI components.
            mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
        }
    }

    /**
     * Represents an asynchronous login/registration task used to authenticate
     * the user.
     */
    public class SyncTask extends AsyncTask<Void, Void, Boolean> {

        private final String token;
        private final InitialSyncActivity parentActivity;

        SyncTask(InitialSyncActivity parentActivity, String token) {
            this.parentActivity = parentActivity;
            this.token = token;
            LogService.initialize(parentActivity.getApplication());
        }

        @Override
        protected Boolean doInBackground(Void... params) {
            boolean success = false;
            try {

                // Fetch my projects
                LogService.debug(ACTIVITY_NAME, "Fetching data from server");
                RequestFuture<JSONArray> projectListFuture = RequestFuture.newFuture();
                JsonArrayRequest fetchProjectListRequest = new JsonArrayRequest(
                        "http://worktajm.com/api/projects/",
                        projectListFuture,
                        projectListFuture) {
                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        HashMap<String, String> headers = new HashMap<>();
                        headers.put("Authorization", "Bearer " + token);
                        return headers;
                    }
                };
                MySingleton.getInstance(parentActivity).addToRequestQueue(fetchProjectListRequest);

                // Fetch my time entries
                RequestFuture<JSONArray> timeEntryListFuture = RequestFuture.newFuture();
                JsonArrayRequest fetchTimeEntriesRequest = new JsonArrayRequest(
                        "http://worktajm.com/api/timeEntries/",
                        timeEntryListFuture,
                        timeEntryListFuture) {
                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        HashMap<String, String> headers = new HashMap<>();
                        headers.put("Authorization", "Bearer " + token);
                        return headers;
                    }
                };
                MySingleton.getInstance(parentActivity).addToRequestQueue(fetchTimeEntriesRequest);

                JSONArray projecsResult = projectListFuture.get(30, TimeUnit.SECONDS);
                storeProjects(projecsResult);

                JSONArray timeEntriesResult = timeEntryListFuture.get(1, TimeUnit.SECONDS);
                storeTimeEntries(timeEntriesResult);

                success = true;
            } catch (InterruptedException e) {
                LogService.error(ACTIVITY_NAME, "InterruptedException: " + e.getMessage());
                e.printStackTrace();
            } catch (ExecutionException e) {
                LogService.error(ACTIVITY_NAME, "ExecutionException: " + e.getMessage());
                e.printStackTrace();
            } catch (TimeoutException e) {
                LogService.error(ACTIVITY_NAME, "TimeoutException: " + e.getMessage());
                e.printStackTrace();
            } catch (JSONException e) {
                LogService.error(ACTIVITY_NAME, "TimeoutException: " + e.getMessage());
                e.printStackTrace();
            }

            LogService.debug(ACTIVITY_NAME, "Done?!");

            return success;
        }

        @Override
        protected void onPostExecute(final Boolean success) {
            LogService.debug(ACTIVITY_NAME, "onPostExecute");
            mSyncTask = null;
            showProgress(false);

            if (success) {
                finish();
                Intent dashboardIntent = new Intent(parentActivity, MainActivity.class);
                parentActivity.startActivity(dashboardIntent);
            } else {
                MySingleton.logout();
                Intent loginIntent = new Intent(parentActivity, LoginActivity.class);
                parentActivity.startActivity(loginIntent);
            }
        }

        @Override
        protected void onCancelled() {
            LogService.debug(ACTIVITY_NAME, "onCancelled");
            mSyncTask = null;
            showProgress(false);
        }


    }

    private void storeProjects(JSONArray result) throws JSONException {
        LogService.debug(ACTIVITY_NAME, "Saving projects to database");
        MySingleton.getProjects().clear();
        for (int i = 0; i < result.length(); i++) {
            JSONObject o = result.getJSONObject(i);
            Project project = new Project(o);
            MySingleton.addProject(project);
            LogService.debug(ACTIVITY_NAME, "Projects: " + project.toString());
        }
    }

    private void storeTimeEntries(JSONArray result) {
        LogService.debug(ACTIVITY_NAME, "Saving time entries to database");
        MySingleton.getTimeEntries().clear();
        for (int i = 0; i < result.length(); i++) {
            try {
                JSONObject o = result.getJSONObject(i);
                TimeEntry timeEntry = new TimeEntry(o);
                MySingleton.addTimeEntry(timeEntry);
                LogService.debug(ACTIVITY_NAME, "Time Entry: " + timeEntry.toString());
            } catch (JSONException e) {
                LogService.error(ACTIVITY_NAME, "Failed to transform json object to time entry. ", e);
            }
        }
    }


}
