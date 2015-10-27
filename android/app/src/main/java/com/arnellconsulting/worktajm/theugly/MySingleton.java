package com.arnellconsulting.worktajm.theugly;

import android.content.Context;
import android.graphics.Bitmap;
import android.util.LruCache;

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.toolbox.ImageLoader;
import com.android.volley.toolbox.Volley;
import com.arnellconsulting.worktajm.model.Me;
import com.arnellconsulting.worktajm.model.Project;
import com.arnellconsulting.worktajm.model.TimeEntry;
import com.arnellconsulting.worktajm.storage.MeRepository;
import com.arnellconsulting.worktajm.storage.TimeEntryRepository;
import com.arnellconsulting.worktajm.utils.LoginResponse;

import net.danlew.android.joda.JodaTimeAndroid;

import org.joda.time.DateTime;
import org.json.JSONException;

import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.ExecutionException;

public class MySingleton {
    private static MySingleton mInstance;
    private static LoginResponse loginResponse;
    private static List<Project> projects = new ArrayList<>();
    private static List<TimeEntry> timeEntries = new ArrayList<>();
    private static Me me = null;
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

        JodaTimeAndroid.init(context);
    }

    public static synchronized MySingleton getInstance(Context context) {
        if (mInstance == null) {
            mInstance = new MySingleton(context);
        }
        return mInstance;
    }

    public static Me getMe() {
        return me;
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

    public static void setMe(final Me me) {
        MySingleton.me = me;
    }

    // Projects
    public static void setProjects(final List<Project> p) { projects = p; }
    public static List<Project> getProjects() {
        return projects;
    }

    // Time Entries
    public static void setTimeEntries(final List<TimeEntry> t) { timeEntries = t; };
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

    public static void stopTimer() {
        String timeEntryId = me.getActiveTimeEntryId();
        if (timeEntryId != null) {
            TimeEntryRepository timeEntryRepository = new TimeEntryRepository(mCtx);
            MeRepository meRepository = new MeRepository(mCtx);
            try {
                TimeEntry timeEntry = timeEntryRepository.read(timeEntryId).get();
                if (timeEntry != null) {
                    timeEntry.setEndTime(DateTime.now());
                    timeEntryRepository.update(timeEntry);
                    me.setActiveTimeEntryId(null);
                    Me me = meRepository.read(null).get();
                    me.setActiveTimeEntryId(null);
                    meRepository.update(me);
                }
            } catch (IOException e) {
                e.printStackTrace();
            } catch (JSONException e) {
                e.printStackTrace();
            } catch (InterruptedException e) {
                e.printStackTrace();
            } catch (ExecutionException e) {
                e.printStackTrace();
            }
        }
    }
}