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
import com.arnellconsulting.worktajm.service.WorktajmService;
import com.arnellconsulting.worktajm.theugly.MySingleton;

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
    public View getView(final int position, View view, ViewGroup parent) {

        final Project project = MySingleton.getProjects().get(position);
        final Me me = MySingleton.getMe();
        final boolean isProjectActive = (me.getActiveProjectId() != null)
                && (me.getActiveProjectId().contentEquals(project.getId()));

        if (inflater == null)
            inflater = (LayoutInflater) activity
                    .getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        final View convertView = (view == null) ? (isProjectActive ? inflater.inflate(R.layout.project_list_row_start, null) : inflater.inflate(R.layout.project_list_row_stop, null)) : view;

        TextView name = (TextView) convertView.findViewById(R.id.name);
        TextView timer = (TextView) convertView.findViewById(R.id.timer);
        TextView total = (TextView) convertView.findViewById(R.id.total);
        Button button = (Button) convertView.findViewById(R.id.button);

        if (isProjectActive) {
            button.setText("Stop");
            button.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    WorktajmService.stopProject(activity, project.getId());
                    me.setActiveProjectId(null);
                    convertView.invalidate();
                    notifyDataSetChanged();
                }
            });
            convertView.setSelected(false);
            button.setBackgroundColor(activity.getResources().getColor(R.color.project_active));
        } else {
            button.setText("Start");
            button.setOnClickListener(new View.OnClickListener() {
                @Override
                public void onClick(View v) {
                    WorktajmService.startProject(activity, project.getId());
                    me.setActiveProjectId(project.getId());
                    convertView.invalidate();
                    notifyDataSetChanged();
                }
            });
            convertView.setSelected(false);
            button.setBackgroundColor(activity.getResources().getColor(R.color.project_inactive));
        }

        name.setText(project.getName());
        timer.setText("");
        total.setText("");

        return convertView;
    }

}