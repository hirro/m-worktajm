package com.arnellconsulting.worktajm.model;

import org.json.JSONException;
import org.json.JSONObject;

public class TimeEntry {
    private String id;
    private String startTime;
    private String endTime;
    private String projectId;

    public TimeEntry(final JSONObject o) throws JSONException {
        this.id = o.getString("_id");
        this.projectId = o.getString("projectId");
        this.startTime = o.getString("startTime");
        this.endTime = o.getString("endTime");

    }

    public String getStartTime() {
        return startTime;
    }

    public void setStartTime(String startTime) {
        this.startTime = startTime;
    }

    public String getEndTime() {
        return endTime;
    }

    public void setEndTime(String endTime) {
        this.endTime = endTime;
    }

    public String getProjectId() {
        return projectId;
    }

    public void setProjectId(String projectId) {
        this.projectId = projectId;
    }

    public String getId() {

        return id;
    }

    public void setId(String id) {
        this.id = id;
    }
}
