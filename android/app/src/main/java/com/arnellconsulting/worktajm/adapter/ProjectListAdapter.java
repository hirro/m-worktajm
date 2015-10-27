package com.arnellconsulting.worktajm.adapter;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.Button;
import android.widget.TextView;

import com.arnellconsulting.worktajm.R;
import com.arnellconsulting.worktajm.model.Me;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.storage.TimeEntryRepository;
import com.arnellconsulting.worktajm.theugly.MySingleton;
import com.arnellconsulting.worktajm.utils.LogService;

import java.util.List;

public class ProjectListAdapter extends BaseAdapter {
    private static final String ACTIVITY_ID = "ProjectListAdapter";
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
    public View getView(final int position, View convertView, ViewGroup parent) {

        if (inflater == null)
            inflater = (LayoutInflater) activity
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        if (convertView == null) {
            convertView = inflater.inflate(R.layout.project_list_row, null);
        }

        TextView name = (TextView) convertView.findViewById(R.id.name);
        TextView timer = (TextView) convertView.findViewById(R.id.timer);
        TextView total = (TextView) convertView.findViewById(R.id.total);
        Button button = (Button) convertView.findViewById(R.id.button);

        // getting movie data for the row
        Project project = MySingleton.getProjects().get(position);

        Me me = MySingleton.getMe();
        if (me.getActiveProjectId().contentEquals(project.getId())) {
            button.setText("Stop");
            //button.setBackgroundColor(activity.getResources().getColor(R.color.project_active));
            // button.setOnClickListener(this);
            convertView.setSelected(true);
            convertView.setBackgroundColor(activity.getResources().getColor(R.color.primary));
        } else {
            button.setText("Start");
            //button.setBackgroundColor(activity.getResources().getColor(R.color.project_inactive));
        }

        button.setOnClickListener(new View.OnClickListener()
        {
            @Override
            public void onClick(View v)
            {
                toggleProject(position);
            }

        });

        name.setText(project.getName());
        timer.setText("");
        total.setText("");

        return convertView;
    }

    void toggleProject(int position) {
        LogService.debug(ACTIVITY_ID, "Toggling " + Integer.toString(position));

        Project project = projects.get(position);
        Me me = MySingleton.getMe();

        if (project.getId().contentEquals(me.getActiveProjectId())) {
            LogService.debug(ACTIVITY_ID, "Stopping project");
            MySingleton.stopTimer();
        } else {
            LogService.debug(ACTIVITY_ID, "Starting project");
            MySingleton.stopTimer(project);
        }
    }

}