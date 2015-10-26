package com.arnellconsulting.worktajm.preferences;

import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

public class PreferenceUtil {

    private static final String USERNAME = "username";
    private static final String PASSWORD = "password";

    public static String getUsername(Context context) {
        SharedPreferences SP = PreferenceManager.getDefaultSharedPreferences(context);
        return SP.getString(USERNAME, null);
    }

    public static void setUsername(Context context, final String username) {
        SharedPreferences SP = PreferenceManager.getDefaultSharedPreferences(context);
        SharedPreferences.Editor editor = SP.edit();
        editor.putString(USERNAME, username);
        editor.commit();
    }

    public static String getPassword(Context context) {
        SharedPreferences SP = PreferenceManager.getDefaultSharedPreferences(context);
        return SP.getString(PASSWORD, null);
    }

    public static void setPassword(Context context, final String password) {
        SharedPreferences SP = PreferenceManager.getDefaultSharedPreferences(context);
        SharedPreferences.Editor editor = SP.edit();
        editor.putString(PASSWORD, password);
        editor.commit();
    }

}
