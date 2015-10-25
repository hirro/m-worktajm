package com.arnellconsulting.worktajm;

import android.app.Activity;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ListView;

import com.arnellconsulting.worktajm.adapter.TimeEntryListAdapter;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.model.TimeEntry;

import java.util.List;


public class LogFragment extends Fragment {

    private ListView listView;
    private TimeEntryListAdapter adapter;

    private OnFragmentInteractionListener mListener;

    public static LogFragment newInstance() {
        LogFragment fragment = new LogFragment();
        Bundle args = new Bundle();
        fragment.setArguments(args);
        return fragment;
    }

    public LogFragment() {
        // Required empty public constructor
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_timer, container, false);

        // Setup list view
        List<TimeEntry> timeEntries = MySingleton.getTimeEntries();
        listView = (ListView) view.findViewById(R.id.listViewProjects);
        adapter = new TimeEntryListAdapter(getActivity(), timeEntries);
        listView.setAdapter(adapter);

        return view;
    }

    public void onButtonPressed(Uri uri) {
        if (mListener != null) {
            mListener.onFragmentInteraction(uri);
        }
    }

    @Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);
        try {
            mListener = (OnFragmentInteractionListener) activity;
        } catch (ClassCastException e) {
            throw new ClassCastException(activity.toString()
                    + " must implement OnFragmentInteractionListener");
        }
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }


}
