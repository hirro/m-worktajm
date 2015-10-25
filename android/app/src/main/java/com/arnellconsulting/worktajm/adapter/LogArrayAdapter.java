package com.arnellconsulting.worktajm.adapter;

import android.content.Context;
import android.support.annotation.NonNull;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.arnellconsulting.worktajm.MySingleton;
import com.arnellconsulting.worktajm.R;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.model.TimeEntry;
import com.arnellconsulting.worktajm.utils.LogService;
import com.sawyer.advadapters.widget.RolodexArrayAdapter;

import org.joda.time.DateTime;
import org.joda.time.Period;
import org.joda.time.format.ISODateTimeFormat;
import org.joda.time.format.PeriodFormatter;
import org.joda.time.format.PeriodFormatterBuilder;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

public class LogArrayAdapter extends RolodexArrayAdapter<Integer, TimeEntry> {
    private static final String ACTIVITY_ID = "TimeEntryRolodexArrayAdapter";
    private Map<String, List<TimeEntry>> groups = new HashMap<>();
    private List<String> indexList = new ArrayList<>();
    private static String DATE_TIME_FORMAT = "yyyyMMdd HH:mm:ss";

    public LogArrayAdapter(Context activity) {
        super(activity);
    }

    @NonNull
    @Override
    public Integer createGroupFor(TimeEntry childItem) {
        String groupName = calculateGroupIdentifier(childItem);
        if (!groups.containsKey(groupName)) {
            groups.put(groupName, new ArrayList<TimeEntry>());
            indexList.add(groupName);
        }
        groups.get(groupName).add(childItem);
        Integer index = indexList.indexOf(groupName);

        return index;
    }

    public void logContents() {
        for (String s : groups.keySet()) {
            LogService.debug(ACTIVITY_ID, "Group: " +s);

            for (TimeEntry timeEntry : groups.get(s)) {
                LogService.debug(ACTIVITY_ID, "TimeEntry: " + timeEntry.toString());
            }
        }
    }

    private String calculateGroupIdentifier(TimeEntry childItem) {
        return childItem.getStartTime().toString(ISODateTimeFormat.date());
    }

    @Override
    protected boolean isChildFilteredOut(TimeEntry childItem, @NonNull CharSequence constraint) {
        return false;
    }

    @Override
    protected boolean isGroupFilteredOut(Integer groupItem, @NonNull CharSequence constraint) {
        return false;
    }

    @NonNull
    @Override
    public View getChildView(@NonNull LayoutInflater inflater, int groupPosition, int childPosition, boolean isLastChild, View convertView, @NonNull ViewGroup parent) {

        if (convertView == null) {
            convertView = inflater.inflate(R.layout.time_entry_child_row, null);
        }

        TextView nameTextView = (TextView) convertView.findViewById(R.id.name);
        TextView durationTextView = (TextView) convertView.findViewById(R.id.duration);

        // Populate the fields
        String groupId = indexList.get(groupPosition);
        TimeEntry timeEntry = groups.get(groupId).get(childPosition);
        Project project = MySingleton.findProject(timeEntry.getProjectId());
        if (project != null) {
            nameTextView.setText(project.getName());
        }

        DateTime startTime = timeEntry.getStartTime();
        DateTime endTime = timeEntry.getEndTime();

        long diff = endTime.getMillis() - startTime.getMillis();
        PeriodFormatter periodFormatter = new PeriodFormatterBuilder()
                .appendDays()
                .appendSuffix("d ")
                .printZeroAlways()
                .minimumPrintedDigits(2)
                .appendHours()
                .appendSuffix(":")
                .printZeroAlways()
                .minimumPrintedDigits(2)
                .appendMinutes()
                .appendSuffix(":")
                .printZeroAlways()
                .minimumPrintedDigits(2)
                .appendSeconds()
                .toFormatter();
        Period period = new Period(startTime.getMillis(), endTime.getMillis());
        String durationString = String.format(
                "%s-%s (%s)",
                startTime.toString("hh:mm:ss"),
                endTime.toString("hh:mm:ss"),
                periodFormatter.print(period));
        durationTextView.setText(durationString);

        return convertView;
    }

    @NonNull
    @Override
    public View getGroupView(@NonNull LayoutInflater inflater, int groupPosition, boolean isExpanded, View convertView, @NonNull ViewGroup parent) {
        if (convertView == null) {
            convertView = inflater.inflate(R.layout.time_entry_group_row, null);
        }

        TextView name = (TextView) convertView.findViewById(R.id.name);

        // Populate the fields
        String label = indexList.get(groupPosition);
        name.setText(label);

        return convertView;
    }
}