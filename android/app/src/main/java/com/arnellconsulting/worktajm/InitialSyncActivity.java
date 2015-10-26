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
import android.widget.TextView;

import com.arnellconsulting.worktajm.model.Me;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.model.TimeEntry;
import com.arnellconsulting.worktajm.storage.MeRepository;
import com.arnellconsulting.worktajm.storage.ProjectRepository;
import com.arnellconsulting.worktajm.storage.TimeEntryRepository;
import com.arnellconsulting.worktajm.theugly.MySingleton;
import com.arnellconsulting.worktajm.utils.LogService;

import org.json.JSONException;

import java.io.IOException;
import java.util.List;
import java.util.concurrent.ExecutionException;
import java.util.concurrent.Future;
import java.util.concurrent.TimeUnit;
import java.util.concurrent.TimeoutException;

public class InitialSyncActivity extends AppCompatActivity {

    /**
     * Keep track of the login task to ensure we can cancel it if requested.
     */
    private SyncTask mSyncTask = null;
    private static final String ACTIVITY_NAME = "InitialSync";
    private View mProgressView;
    private TextView mProgressText;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_initial_sync);
        Toolbar toolbar = (Toolbar) findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);
        mProgressView =  findViewById(R.id.projectProgressBar);
        mProgressText = (TextView) findViewById(R.id.projectProgressText);

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
                setProgressText(R.string.progress_project_progress);
                ProjectRepository ps = new ProjectRepository(parentActivity.getBaseContext());
                Future<List<Project>> x = ps.list();
                MySingleton.setProjects(x.get(30, TimeUnit.SECONDS));

                setProgressText(R.string.progress_time_entries_progress);
                TimeEntryRepository tes = new TimeEntryRepository(parentActivity.getBaseContext());
                Future<List<TimeEntry>> y = tes.list();
                MySingleton.setTimeEntries(y.get(30, TimeUnit.SECONDS));

                setProgressText(R.string.progress_me_progress);
                MeRepository mes = new MeRepository(parentActivity.getBaseContext());
                Future<Me> z = mes.read(null);
                MySingleton.setMe(z.get(30, TimeUnit.SECONDS));

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
            } catch (IOException e) {
                LogService.error(ACTIVITY_NAME, "IOException: " + e.getMessage());
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

        private void setProgressText(final int text) {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    mProgressText.setText(text);
                }
            });
        }


    }


}
