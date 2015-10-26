package com.arnellconsulting.worktajm.storage;

import android.content.Context;

import com.android.volley.AuthFailureError;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.JsonObjectRequest;
import com.android.volley.toolbox.RequestFuture;
import com.arnellconsulting.worktajm.theugly.MySingleton;
import com.arnellconsulting.worktajm.model.Me;
import com.arnellconsulting.worktajm.utils.LogService;
import com.arnellconsulting.worktajm.utils.LoginResponse;

import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.concurrent.Future;
import java.util.concurrent.TimeUnit;

public class MeRepository extends Repository<Me> {

    private static String ACTIVITY_NAME = "MeStorage";

    public MeRepository(Context c) {
        super(c);
    }

    @Override
    public Future<String> create(Me me) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<Me> read(String id) throws IOException, JSONException {
        RequestFuture<Me> result = RequestFuture.newFuture();

        LogService.debug(ACTIVITY_NAME, "Getting timeEntries");

        try {
            final LoginResponse loginResponse = MySingleton.getLoginResponse();

            LogService.debug(ACTIVITY_NAME, "Creating login request");
            HashMap<String, String> httpHeaders = new HashMap<String, String>();

            if (loginResponse == null) {
                result.onErrorResponse(new VolleyError());
            } else {
                LogService.debug(ACTIVITY_NAME, "Fetching data from server");

                RequestFuture<JSONObject> future = RequestFuture.newFuture();
                JsonObjectRequest jsObjRequest = new JsonObjectRequest(
                        "http://worktajm.com/api/users/me",
                        null,
                        future,
                        future) {
                    @Override
                    public Map<String, String> getHeaders() throws AuthFailureError {
                        HashMap<String, String> headers = new HashMap<>();
                        headers.put("Authorization", "Bearer " + loginResponse.getAuthToken());
                        headers.put("Connection", "close");
                        return headers;
                    }
                };

                MySingleton.getInstance(context).addToRequestQueue(jsObjRequest);
                JSONObject meResult = future.get(30, TimeUnit.SECONDS);
                Me me = new Me(meResult);
                result.onResponse(me);
            }
        } catch (Exception e) {
            LogService.error(ACTIVITY_NAME, "Exception: " + e.getMessage());
            result.onErrorResponse(new VolleyError(e));
        }

        return result;
    }

    @Override
    public Future<Me> update(Me me) throws IOException, JSONException {
        return null;
    }

    @Override
    public Future<Me> delete(String id) throws IOException, JSONException {
        throw new RuntimeException("Not allowed operation");
    }

    @Override
    public Future<List<Me>> list() throws IOException, JSONException {
        throw new RuntimeException("There can only be one");
    }

}
