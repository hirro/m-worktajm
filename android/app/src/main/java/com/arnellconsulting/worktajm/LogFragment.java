package com.arnellconsulting.worktajm;

import android.app.Activity;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ExpandableListView;

import com.arnellconsulting.worktajm.adapter.LogArrayAdapter;
import com.arnellconsulting.worktajm.model.TimeEntry;

import java.util.List;


public class LogFragment extends Fragment {

    private ExpandableListView listView;
    private LogArrayAdapter adapter;

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
        View view = inflater.inflate(R.layout.fragment_log, container, false);

        // Setup list view
        List<TimeEntry> timeEntries = MySingleton.getTimeEntries();
        listView = (ExpandableListView) view.findViewById(R.id.listViewTimeEntries);
        adapter = new LogArrayAdapter(getActivity());
        adapter.addAll(timeEntries);
        listView.setAdapter(adapter);
        adapter.logContents();

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
