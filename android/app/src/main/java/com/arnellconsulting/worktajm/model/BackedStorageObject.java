package com.arnellconsulting.worktajm.model;

/**
 * Locally stored objects backed by Mongodb.
 *
 * Contains generic properties for locally synchronized mongodb entries.
 */
public abstract class BackedStorageObject<T> {
    volatile boolean isDirty;
    volatile boolean isNew;

    public String getId() {
        return id;
    }

    public void setId(String id) {
        this.id = id;
    }

    public boolean isNew() {
        return isNew;
    }

    public void setIsNew(boolean isNew) {
        this.isNew = isNew;
    }

    public boolean isDirty() {
        return isDirty;
    }

    public void setIsDirty(boolean isDirty) {
        this.isDirty = isDirty;
    }

    private String id;

    protected BackedStorageObject() {
        isDirty = false;
        isNew = false;
    }

}
