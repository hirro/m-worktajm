package com.arnellconsulting.worktajm.model;

import org.json.JSONException;
import org.json.JSONObject;

public class Me {
    private String id;
    private String activeProjectId;
    private String activeTimeEntryId;
    private String email;
    private String name;

    public Me(JSONObject o) throws JSONException {
        this.id = o.getString("_id");
        this.activeProjectId = o.getString("activeProjectId");
        this.activeTimeEntryId = o.getString("activeTimeEntryId");
        this.email = o.getString("email");
        this.name = o.getString("name");
    }

    public String getActiveProjectId() {
        return activeProjectId;
    }

    public void setActiveProjectId(String activeProjectId) {
        this.activeProjectId = activeProjectId;
    }

    public String getActiveTimeEntryId() {
        return activeTimeEntryId;
    }

    public void setActiveTimeEntryId(String activeTimeEntryId) {
        this.activeTimeEntryId = activeTimeEntryId;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
