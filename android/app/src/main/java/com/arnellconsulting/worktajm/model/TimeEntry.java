package com.arnellconsulting.worktajm.model;

import org.joda.time.DateTime;
import org.json.JSONException;
import org.json.JSONObject;

public class TimeEntry {
    private String id;
    private DateTime startTime;
    private DateTime endTime;
    private String projectId;

    public TimeEntry(final JSONObject o) throws JSONException {
        this.id = o.getString("_id");
        this.projectId = o.getString("projectId");
        this.startTime = DateTime.parse(o.getString("startTime"));
        this.endTime = DateTime.parse(o.getString("endTime"));
    }

    public DateTime getStartTime() {
        return startTime;
    }

    public void setStartTime(DateTime startTime) {
        this.startTime = startTime;
    }

    public DateTime getEndTime() {
        return endTime;
    }

    public void setEndTime(DateTime endTime) {
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

    public String toString() {
        StringBuffer sb = new StringBuffer();
        sb.append("{");

        sb.append("_id: \"");
        sb.append(getId());
        sb.append("\", ");

        sb.append("startTime: \"");
        sb.append(getStartTime().toLocalDateTime().toString());
        sb.append("\", ");

        sb.append("endTime: \"");
        sb.append(getEndTime().toLocalDateTime().toString());
        sb.append("\", ");

        sb.append("projectId: \"");
        sb.append(getProjectId());

        sb.append("}");

        return sb.toString();
    }
}
