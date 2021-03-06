package com.arnellconsulting.worktajm.utils;

import android.content.Context;
import android.util.Log;

import com.logentries.android.AndroidLogger;

import java.io.PrintWriter;
import java.io.StringWriter;

public class LogService {

    private static AndroidLogger LOGGER = null;

    public static AndroidLogger getLogger() {
        return LOGGER;
    }

    public static void initialize(Context applicationContext) {
        LOGGER = AndroidLogger.getLogger(applicationContext, "8455a5a9-7cde-4a41-8483-e273c7f255d9", false);
    }

    public static void debug(String activity, String message) {
        Log.d(activity, message);
        if (LOGGER != null) {
            LOGGER.debug(message);
            LOGGER.flushConnection();
        }
    }

    public static void debug(String activity, String message, Throwable t) {
        Log.d(activity, message, t);
        if (LOGGER != null) {
            LOGGER.debug(message);

            StringWriter errors = new StringWriter();
            t.printStackTrace(new PrintWriter(errors));
            LOGGER.error(errors.toString());

            LOGGER.flushConnection();
        }
    }

    public static void error(String activity, String message) {
        Log.e(activity, message);
        if (LOGGER != null) {
            LOGGER.error(message);
            LOGGER.flushConnection();
        }
    }

    public static void error(String activity, String message, Throwable t) {
        Log.e(activity, message, t);
        if (LOGGER != null) {
            LOGGER.error(message);

            StringWriter errors = new StringWriter();
            t.printStackTrace(new PrintWriter(errors));
            LOGGER.error(errors.toString());

            LOGGER.flushConnection();
        }
    }

}
