package com.arnellconsulting.worktajm.storage;

import android.content.Context;

import com.android.volley.AuthFailureError;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.RequestFuture;
import com.arnellconsulting.worktajm.theugly.MySingleton;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.utils.LogService;
import com.arnellconsulting.worktajm.utils.LoginResponse;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.Future;
import java.util.concurrent.TimeUnit;

public class ProjectRepository extends Repository<Project> {

    private static String ACTIVITY_NAME = "ProjectStorage";

    public ProjectRepository(Context c) {
        super(c);
    }

    @Override
    public Future<String> create(Project project) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<Project> read(String id) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<Project> update(Project project) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<Project> delete(String id) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<List<Project>> list() throws IOException, JSONException {
        RequestFuture<List<Project>> result = RequestFuture.newFuture();

        LogService.debug(ACTIVITY_NAME, "Getting projects");

        try {
            final LoginResponse loginResponse = MySingleton.getLoginResponse();
            RequestFuture<JSONArray> projectListFuture = RequestFuture.newFuture();
            if (loginResponse == null) {
                result.onErrorResponse(new VolleyError());
            } else {
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
                JSONArray projecsResult = projectListFuture.get(30, TimeUnit.SECONDS);
                List<Project> projectList = new ArrayList<>();
                for (int i = 0; i < projecsResult.length(); i++) {
                    try {
                        JSONObject o = projecsResult.getJSONObject(i);
                        Project project = new Project(o);
                        projectList.add(project);
                        LogService.debug(ACTIVITY_NAME, "Project: " + project.toString());
                    } catch (JSONException e) {
                        LogService.error(ACTIVITY_NAME, "Failed to transform json object to project. ", e);
                    }
                }

                result.onResponse(projectList);

            }

        } catch (Exception e) {
            LogService.error(ACTIVITY_NAME, "Exception: " + e.getMessage());
            result.onErrorResponse(new VolleyError(e));
        }

        LogService.debug(ACTIVITY_NAME, "list started");


        return result;
    }

}
