package com.arnellconsulting.worktajm.storage;

import android.content.Context;

import org.json.JSONException;

import java.io.IOException;
import java.util.List;
import java.util.concurrent.Future;

/**
 * Storage class
 */
public abstract class Repository<T> {

    /**
     * Creates an remote object.
     *
     * @param t object to create.
     * @return The object identifier of the newly created object.
     * @throws IOException on database or network error.
     * @throws JSONException on JSON related errors.
     */
    protected abstract Future<String> create(T t) throws IOException, JSONException;

    /**
     * Retrieves the object from the remote database.
     *
     * @param id the object identifier of the remote object.
     * @return The object if found, null otherwise.
     * @throws IOException on database or network error.
     * @throws JSONException on JSON related errors.
     */
    protected abstract Future<T> read(final String id) throws IOException, JSONException;

    /**
     * Updates the provided object in the remote storage/
     * @param t the object to update.
     * @return the updated object
     * @throws IOException on database or network error.
     * @throws JSONException on JSON related errors.
     */
    protected abstract Future<T> update(T t) throws IOException, JSONException;

    /**
     * Deletes the provided object from the remote storage.
     * @param id the object identifier of the object to remove.
     * @return
     * @throws IOException on database or network error.
     * @throws JSONException on JSON related errors.
     */
    protected abstract Future<T> delete(final String id) throws IOException, JSONException;

    /**
     * Retrieves all the remote objects, may be a bit slow if running the first time.
     * @return list of all remote objects of this type.
     * @throws IOException on database or network error.
     * @throws JSONException on JSON related errors.
     */
    protected abstract Future<List<T>> list(final Context context) throws IOException, JSONException;
};