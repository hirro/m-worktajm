package com.arnellconsulting.worktajm.adapter;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.arnellconsulting.worktajm.theugly.MySingleton;
import com.arnellconsulting.worktajm.R;
import com.arnellconsulting.worktajm.model.Project;

import java.util.List;

public class ProjectListAdapter extends BaseAdapter {
    private Activity activity;
    private LayoutInflater inflater;
    private List<Project> projects;

    public ProjectListAdapter(Activity activity, List<Project> projects) {
        this.activity = activity;
        this.projects = projects;
    }

    @Override
    public int getCount() {
        return projects.size();
    }

    @Override
    public Object getItem(int location) {
        return projects.get(location);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        if (inflater == null)
            inflater = (LayoutInflater) activity
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        if (convertView == null)
            convertView = inflater.inflate(R.layout.project_list_row, null);

        TextView name = (TextView) convertView.findViewById(R.id.name);
        TextView timer = (TextView) convertView.findViewById(R.id.timer);
        TextView total = (TextView) convertView.findViewById(R.id.total);

        // getting movie data for the row
        Project project = MySingleton.getProjects().get(position);

        name.setText(project.getName());
        timer.setText("");
        total.setText("");

        return convertView;
    }

}