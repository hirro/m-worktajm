package com.arnellconsulting.worktajm.storage;

import android.content.Context;

import com.android.volley.AuthFailureError;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonArrayRequest;
import com.android.volley.toolbox.RequestFuture;
import com.arnellconsulting.worktajm.theugly.MySingleton;
import com.arnellconsulting.worktajm.model.TimeEntry;
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

public class TimeEntryRepository extends Repository<TimeEntry> {

    private static String ACTIVITY_NAME = "TimeEntryStorage";

    public TimeEntryRepository(Context c) {
        super(c);
    }

    @Override
    public Future<String> create(TimeEntry timeEntry) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<TimeEntry> read(String id) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<TimeEntry> update(TimeEntry timeEntry) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<TimeEntry> delete(String id) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<List<TimeEntry>> list() throws IOException, JSONException {
        RequestFuture<List<TimeEntry>> result = RequestFuture.newFuture();

        LogService.debug(ACTIVITY_NAME, "Getting timeEntries");

        try {
            final LoginResponse loginResponse = MySingleton.getLoginResponse();
            RequestFuture<JSONArray> timeEntryListFuture = RequestFuture.newFuture();
            if (loginResponse == null) {
                result.onErrorResponse(new VolleyError());
            } else {
                // Fetch my timeEntrys
                LogService.debug(ACTIVITY_NAME, "Fetching data from server");
                JsonArrayRequest TimeEntryListRequest = new JsonArrayRequest(
                        "http://worktajm.com/api/timeEntries/",
                        timeEntryListFuture,
                        timeEntryListFuture) {
                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        HashMap<String, String> headers = new HashMap<>();
                        headers.put("Authorization", "Bearer " + loginResponse.getAuthToken());
                        return headers;
                    }
                };
                MySingleton.getInstance(context).addToRequestQueue(TimeEntryListRequest);
                JSONArray projecsResult = timeEntryListFuture.get(30, TimeUnit.SECONDS);
                List<TimeEntry> timeEntryList = new ArrayList<>();
                for (int i = 0; i < projecsResult.length(); i++) {
                    try {
                        JSONObject o = projecsResult.getJSONObject(i);
                        TimeEntry timeEntry = new TimeEntry(o);
                        timeEntryList.add(timeEntry);
                        LogService.debug(ACTIVITY_NAME, "TimeEntry: " + timeEntry.toString());
                    } catch (JSONException e) {
                        LogService.error(ACTIVITY_NAME, "Failed to transform json object to timeEntry. ", e);
                    }
                }

                result.onResponse(timeEntryList);
            }
        } catch (Exception e) {
            LogService.error(ACTIVITY_NAME, "Exception: " + e.getMessage());
            result.onErrorResponse(new VolleyError(e));
        }

        return result;
    }

}
