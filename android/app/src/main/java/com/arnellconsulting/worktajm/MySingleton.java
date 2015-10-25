package com.arnellconsulting.worktajm;

import android.content.Context;
import android.graphics.Bitmap;
import android.util.LruCache;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.ImageLoader;
import com.android.volley.toolbox.Volley;
import com.arnellconsulting.worktajm.model.TimeEntry;
import com.arnellconsulting.worktajm.utils.LoginResponse;
import com.arnellconsulting.worktajm.model.Project;

import java.util.ArrayList;
import java.util.List;

public class MySingleton {
    private static MySingleton mInstance;
    private static LoginResponse loginResponse;
    private static List<Project> projects = new ArrayList<>();
    private static List<TimeEntry> timeEntries = new ArrayList<>();
    private RequestQueue mRequestQueue;
    private ImageLoader mImageLoader;
    private static Context mCtx;

    private MySingleton(Context context) {
        mCtx = context;
        mRequestQueue = getRequestQueue();

        mImageLoader = new ImageLoader(mRequestQueue,
                new ImageLoader.ImageCache() {
                    private final LruCache<String, Bitmap>
                            cache = new LruCache<String, Bitmap>(20);

                    @Override
                    public Bitmap getBitmap(String url) {
                        return cache.get(url);
                    }

                    @Override
                    public void putBitmap(String url, Bitmap bitmap) {
                        cache.put(url, bitmap);
                    }
                });
    }

    public static synchronized MySingleton getInstance(Context context) {
        if (mInstance == null) {
            mInstance = new MySingleton(context);
        }
        return mInstance;
    }

    public RequestQueue getRequestQueue() {
        if (mRequestQueue == null) {
            // getApplicationContext() is key, it keeps you from leaking the
            // Activity or BroadcastReceiver if someone passes one in.
            mRequestQueue = Volley.newRequestQueue(mCtx.getApplicationContext());
        }
        return mRequestQueue;
    }

    public <T> void addToRequestQueue(Request<T> req) {
        getRequestQueue().add(req);
    }

    public ImageLoader getImageLoader() {
        return mImageLoader;
    }

    public static void setLoginResponse(final LoginResponse loginResponse) {
        MySingleton.loginResponse = loginResponse;
    }

    public static boolean isLoggedIn() {
        return MySingleton.loginResponse != null;
    }

    public static void logout() {
        MySingleton.loginResponse = null;
    }

    public static LoginResponse getLoginResponse() {
        return MySingleton.loginResponse;
    }

    // Projects
    public static void addProject(Project project) {
        projects.add(project);
    }
    public static List<Project> getProjects() {
        return projects;
    }

    // Time Entries
    public static void addTimeEntry(TimeEntry timeEntry) {
        timeEntries.add(timeEntry);
    }
    public static List<TimeEntry> getTimeEntries() {
        return timeEntries;
    }

    public static Project findProject(String projectId) {
        for (Project p : projects) {
            if (p.getId().contentEquals(projectId)) {
                return p;
            }
        }
        return null;
    }
}