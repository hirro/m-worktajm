package com.arnellconsulting.worktajm.storage;

import android.content.Context;

import com.android.volley.AuthFailureError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.RequestFuture;
import com.arnellconsulting.worktajm.MySingleton;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.model.TimeEntry;
import com.arnellconsulting.worktajm.utils.LogService;
import com.arnellconsulting.worktajm.utils.LoginResponse;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.Future;
import java.util.concurrent.TimeUnit;

public class ProjectStorage extends Repository<Project> {

    private static String ACTIVITY_NAME = "ProjectStorage";

    @Override
    protected Future<String> create(Project project) throws IOException, JSONException {
        return null;
    }

    @Override
    protected Future<Project> read(String id) throws IOException, JSONException {
        return null;
    }

    @Override
    protected Future<Project> update(Project project) throws IOException, JSONException {
        return null;
    }

    @Override
    protected Future<Project> delete(String id) throws IOException, JSONException {
        return null;
    }

    @Override
    protected Future<List<Project>> list(final Context context) throws IOException, JSONException {
        RequestFuture<List<Project>> result = RequestFuture.newFuture();

        LogService.debug(ACTIVITY_NAME, "Getting projects");

        try {
            final LoginResponse loginResponse = MySingleton.getLoginResponse();
            RequestFuture<JSONArray> projectListFuture = RequestFuture.newFuture();
            if (loginResponse != null) {
                // Fetch my projects
                LogService.debug(ACTIVITY_NAME, "Fetching data from server");
                JsonArrayRequest fetchProjectListRequest = new JsonArrayRequest(
                        "http://worktajm.com/api/projects/",
                        projectListFuture,
                        projectListFuture) {
                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        HashMap<String, String> headers = new HashMap<>();
                        headers.put("Authorization", "Bearer " + loginResponse.getAuthToken());
                        return headers;
                    }
                };
                MySingleton.getInstance(context).addToRequestQueue(fetchProjectListRequest);
            }
            JSONArray projecsResult = projectListFuture.get(30, TimeUnit.SECONDS);

        } catch (Exception e) {
            LogService.error(ACTIVITY_NAME, "Exception: " + e.getMessage());
        }

        LogService.debug(ACTIVITY_NAME, "Done?!");


        return result;
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
