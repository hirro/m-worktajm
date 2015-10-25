package com.arnellconsulting.worktajm.model;

import org.json.JSONException;
import org.json.JSONObject;

public class Project {
    private String id;

    public Project(JSONObject o) throws JSONException {
        this.id = o.getString("_id");
        this.name = o.getString("name");
    }

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    private String name;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
