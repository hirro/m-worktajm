package com.arnellconsulting.worktajm.storage;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;

import com.arnellconsulting.worktajm.model.Project;

import java.util.List;

public class ProjectOpenHelper extends SQLiteOpenHelper {

    private static final int DATABASE_VERSION = 2;
    private static final String DICTIONARY_TABLE_CREATE =
            "CREATE TABLE PROJECT (" +
                    "id TEXT, " +
                    ");";
    private static final String DATABASE_NAME = "WorktajmDb";

    ProjectOpenHelper(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL(DICTIONARY_TABLE_CREATE);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {

    }

    public void importProjects(List<Project> projects) {

    }
}