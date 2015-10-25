package com.arnellconsulting.worktajm.utils;

import org.json.JSONException;
import org.json.JSONObject;

public class LoginResponse {
    private void setAuthToken(String authToken) {
        this.authToken = authToken;
    }

    public String getAuthToken() {
        return authToken;
    }

    private String authToken;
    public static LoginResponse create(JSONObject json) {
        LoginResponse response = new LoginResponse();
        try {
            String token = json.getString("token");
            response.setAuthToken(token);
        } catch (JSONException e) {
        }
        return response;
    }
}
