package com.arnellconsulting.worktajm.service;

import android.app.IntentService;
import android.content.Context;
import android.content.Intent;

import com.arnellconsulting.worktajm.utils.LogService;

public class WorktajmService extends IntentService {

    private static String LOG_ID = WorktajmService.class.getName();

    /**
     * Creates an IntentService.  Invoked by your subclass's constructor.
     *
     * @param name Used to name the worker thread, important only for debugging.
     */
    public WorktajmService(String name) {
        super(name);
    }

    /**
     * Utility method to start a project with the given project id
     * @param projectId
     */
    public static void startProject(
            final Context ctx,
            final String projectId) {
        LogService.debug(LOG_ID, "Starting project: " + projectId);
    }

    public static void stopProject(
            final Context ctx,
            final String projectId) {
        LogService.debug(LOG_ID, "Stopping project: " + projectId);
    }

    @Override
    public void onCreate() {
        LogService.debug(LOG_ID, "onCreate");
    }

    @Override
    protected void onHandleIntent(Intent intent) {
        LogService.debug(LOG_ID, "onHandleIntent: " + intent.getAction());
    }

    private void startProject(final String projectId) {

    }

    private void stopProject(final String projectId) {

    }
}
