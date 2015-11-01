package com.arnellconsulting.worktajm;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.app.LoaderManager.LoaderCallbacks;
import android.content.CursorLoader;
import android.content.Intent;
import android.content.Loader;
import android.database.Cursor;
import android.net.Uri;
import android.os.AsyncTask;
import android.os.Build;
import android.os.Bundle;
import android.provider.ContactsContract;
import android.support.v7.app.ActionBarActivity;
import android.text.TextUtils;
import android.view.KeyEvent;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.inputmethod.EditorInfo;
import android.widget.ArrayAdapter;
import android.widget.AutoCompleteTextView;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ScrollView;
import android.widget.TextView;

import com.afollestad.materialdialogs.MaterialDialog;
import com.android.volley.AuthFailureError;
import com.android.volley.Request;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.RequestFuture;
import com.arnellconsulting.worktajm.preferences.PreferenceUtil;
import com.arnellconsulting.worktajm.theugly.MySingleton;
import com.arnellconsulting.worktajm.utils.LogService;
import com.arnellconsulting.worktajm.utils.LoginResponse;

import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.TimeUnit;

/**
 * A login screen that offers login via email/password.
 */
public class  LoginActivity extends ActionBarActivity implements LoaderCallbacks<Cursor> {

    private static final String ACTIVITY_NAME = "LoginActivity";

    /**
     * Keep track of the login task to ensure we can cancel it if requested.
     */
    private UserLoginTask mAuthTask = null;

    // UI references.
    private AutoCompleteTextView mEmailView;
    private EditText mPasswordView;
    private ScrollView mLoginFormView;
    private MaterialDialog materialDialog;

    public LoginActivity() {
        super();
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_login);

        // Set up the login form.
        mEmailView = (AutoCompleteTextView) findViewById(R.id.email);
        populateAutoComplete();

        mLoginFormView = (ScrollView) findViewById(R.id.login_form);
        mPasswordView = (EditText) findViewById(R.id.password);
        final Button mEmailSignInButton = (Button) findViewById(R.id.email_sign_in_button);

        mPasswordView.setOnEditorActionListener(new TextView.OnEditorActionListener() {
            @Override
            public boolean onEditorAction(TextView textView, int id, KeyEvent keyEvent) {
                if (id == R.id.login || id == EditorInfo.IME_NULL) {
                    attemptLogin();
                    return true;
                }
                return false;
            }
        });

        String password = PreferenceUtil.getPassword(getBaseContext());
        String email = PreferenceUtil.getUsername(getBaseContext());
        if (password != null) {
            mPasswordView.setText(password);
        }
        if (email != null) {
            mEmailView.setText(email);
        }

        mEmailSignInButton.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View view) {
                attemptLogin();
            }
        });

        LogService.initialize(this.getBaseContext());
    }

    private void populateAutoComplete() {
        getLoaderManager().initLoader(0, null, this);
    }


    /**
     * Attempts to sign in or register the account specified by the login form.
     * If there are form errors (invalid email, missing fields, etc.), the
     * errors are presented and no actual login attempt is made.
     */
    private void attemptLogin() {
        if (mAuthTask != null) {
            return;
        }

        // Reset errors.
        mEmailView.setError(null);
        mPasswordView.setError(null);

        // Store values at the time of the login attempt.
        String email = mEmailView.getText().toString().trim();
        String password = mPasswordView.getText().toString().trim();

        boolean cancel = false;
        View focusView = null;

        // Check for a valid password, if the user entered one.
        if (!TextUtils.isEmpty(password) && !isPasswordValid(password)) {
            mPasswordView.setError(getString(R.string.error_invalid_password));
            focusView = mPasswordView;
            cancel = true;
        }

        // Check for a valid email address.
        if (TextUtils.isEmpty(email)) {
            mEmailView.setError(getString(R.string.error_field_required));
            focusView = mEmailView;
            cancel = true;
        } else if (!isEmailValid(email)) {
            mEmailView.setError(getString(R.string.error_invalid_email));
            focusView = mEmailView;
            cancel = true;
        }

        if (cancel) {
            // There was an error; don't attempt login and focus the first
            // form field with an error.
            focusView.requestFocus();
        } else {
            // Show a progress spinner, and kick off a background task to
            // perform the user login attempt.
            showProgress(true);
            mAuthTask = new UserLoginTask(this, email, password);
            mAuthTask.execute((Void) null);
        }
    }

    private boolean isEmailValid(String email) {
        //TODO: Replace this with your own logic
        return email.contains("@");
    }

    private boolean isPasswordValid(String password) {
        //TODO: Replace this with your own logic
        return password.length() >= 4;
    }

    /**
     * Shows the progress UI and hides the login form.
     */
    private void showProgress(final boolean show) {
        if (show) {
            materialDialog = new MaterialDialog.Builder(this)
                    .title(R.string.login_in_progress)
                    .content(R.string.please_wait)
                    .progress(true, 0)
                    .show();
        } else if (materialDialog != null) {
            materialDialog.hide();
        }
    }

    @Override
    public Loader<Cursor> onCreateLoader(int i, Bundle bundle) {
        return new CursorLoader(this,
                // Retrieve data rows for the device user's 'profile' contact.
                Uri.withAppendedPath(ContactsContract.Profile.CONTENT_URI,
                        ContactsContract.Contacts.Data.CONTENT_DIRECTORY), ProfileQuery.PROJECTION,

                // Select only email addresses.
                ContactsContract.Contacts.Data.MIMETYPE +
                        " = ?", new String[]{ContactsContract.CommonDataKinds.Email
                .CONTENT_ITEM_TYPE},

                // Show primary email addresses first. Note that there won't be
                // a primary email address if the user hasn't specified one.
                ContactsContract.Contacts.Data.IS_PRIMARY + " DESC");
    }

    @Override
    public void onLoadFinished(Loader<Cursor> cursorLoader, Cursor cursor) {
        List<String> emails = new ArrayList<>();
        cursor.moveToFirst();
        while (!cursor.isAfterLast()) {
            emails.add(cursor.getString(ProfileQuery.ADDRESS));
            cursor.moveToNext();
        }

        addEmailsToAutoComplete(emails);
    }

    @Override
    public void onLoaderReset(Loader<Cursor> cursorLoader) {

    }

    private interface ProfileQuery {
        String[] PROJECTION = {
                ContactsContract.CommonDataKinds.Email.ADDRESS,
                ContactsContract.CommonDataKinds.Email.IS_PRIMARY,
        };

        int ADDRESS = 0;
        int IS_PRIMARY = 1;
    }


    private void addEmailsToAutoComplete(List<String> emailAddressCollection) {
        //Create adapter to tell the AutoCompleteTextView what to show in its dropdown list.
        ArrayAdapter<String> adapter =
                new ArrayAdapter<>(LoginActivity.this,
                        android.R.layout.simple_dropdown_item_1line, emailAddressCollection);

        mEmailView.setAdapter(adapter);
    }

    /**
     * Represents an asynchronous login/registration task used to authenticate
     * the user.
     */
    public class UserLoginTask extends AsyncTask<Void, Void, Boolean> {

        private final String email;
        private final String password;
        private final LoginActivity parentActivity;

        UserLoginTask(LoginActivity parentActivity, String email, String password) {
            this.parentActivity = parentActivity;
            this.email = email;
            this.password = password;
            LogService.initialize(parentActivity.getApplication());
        }

        @Override
        protected Boolean doInBackground(Void... params) {
            boolean success = false;
            try {
                LogService.debug(ACTIVITY_NAME, "Creating login request");
                HashMap<String, String> httpHeaders = new HashMap<String, String>();

                JSONObject request = new JSONObject();
                request.put("email", email);
                request.put("password", password);

                RequestFuture<JSONObject> future = RequestFuture.newFuture();
                JsonObjectRequest jsObjRequest = new JsonObjectRequest(
                        Request.Method.POST,
                        "http://worktajm.com/auth/local",
                        request,
                        future,
                        future) {
                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        HashMap<String, String> headers = new HashMap<>();
                        headers.put("CUSTOM_HEADER", "Yahoo");
                        headers.put("ANOTHER_CUSTOM_HEADER", "Google");
                        return headers;
                    }
                };
                MySingleton.getInstance(parentActivity).addToRequestQueue(jsObjRequest);

                JSONObject result = future.get(30, TimeUnit.SECONDS);
                LogService.debug(ACTIVITY_NAME, "Response: " + result.toString());
                PreferenceUtil.setPassword(getBaseContext(), password);
                PreferenceUtil.setUsername(getBaseContext(), email);
                LoginResponse loginResponse = LoginResponse.create(result);
                if (loginResponse != null) {
                    MySingleton.setLoginResponse(loginResponse);
                }
                success = true;

            } catch (JSONException e) {
                LogService.error(ACTIVITY_NAME, "JSONException: " + e.getMessage());
            } catch (Exception e) {
                LogService.error(ACTIVITY_NAME, "Exception: " + e.getMessage());
            }

            LogService.debug(ACTIVITY_NAME, "Done?!");

            return success;
        }

        @Override
        protected void onPostExecute(final Boolean success) {
            LogService.debug(ACTIVITY_NAME, "onPostExecute");
            mAuthTask = null;
            showProgress(false);

            if (success) {
                finish();
                Intent dashboardIntent = new Intent(parentActivity, InitialSyncActivity.class);
                parentActivity.startActivity(dashboardIntent);
            } else {
                mPasswordView.setError(getString(R.string.error_incorrect_password));
                mPasswordView.requestFocus();
            }
        }

        @Override
        protected void onCancelled() {
            LogService.debug(ACTIVITY_NAME, "onCancelled");
            mAuthTask = null;
            showProgress(false);
        }


    }
}

