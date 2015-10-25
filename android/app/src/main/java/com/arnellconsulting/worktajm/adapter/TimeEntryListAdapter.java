package com.arnellconsulting.worktajm.adapter;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.arnellconsulting.worktajm.MySingleton;
import com.arnellconsulting.worktajm.R;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.model.TimeEntry;

import java.util.List;

public class TimeEntryListAdapter extends BaseAdapter {
    private Activity activity;
    private LayoutInflater inflater;
    private List<Project> projects;
    private List<TimeEntry> timeEntries;

    public TimeEntryListAdapter(
            final Activity activity,
            final List<TimeEntry> timeEntries) {
        this.activity = activity;
        this.timeEntries = timeEntries;
    }

    @Override
    public int getCount() {
        return timeEntries.size();
    }

    @Override
    public Object getItem(int location) {
        return timeEntries.get(location);
    }

    @Override
    public long getItemId(int position) {
        return position;
    }

    @Override
    public View getView(int position, View convertView, ViewGroup parent) {

        if (inflater == null) {
            inflater = (LayoutInflater) activity.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
        }
        if (convertView == null) {
            convertView = inflater.inflate(R.layout.time_entry_list_row, null);
        }

        // Find controls
        TextView name = (TextView) convertView.findViewById(R.id.name);
        TextView endTime = (TextView) convertView.findViewById(R.id.endTime);
        TextView startTime = (TextView) convertView.findViewById(R.id.startTime);

        // Populate the fields
        TimeEntry timeEntry = timeEntries.get(position);
        Project project = MySingleton.findProject(timeEntry.getProjectId());
        if (project != null) {
            name.setText(project.getName());
        }
        endTime.setText(timeEntry.getEndTime());
        startTime.setText(timeEntry.getStartTime());

        return convertView;
    }

}