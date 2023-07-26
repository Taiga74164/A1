using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asyncoroutine;
using UnityEngine;
using Object = UnityEngine.Object;

/// <summary>
/// Manages assets in the `Resources` directory.
/// </summary>
public class ResourceManager : Singleton<ResourceManager>
{
    /// <summary>
    /// Determines whether the startup load should be asynchronous.
    /// </summary>
    public bool AsyncStartup;

    /// <summary>
    /// This is a list of all resources that should be loaded on startup.
    /// </summary>
    public List<string> StartupLoad = new();

    /// <summary>
    /// This is the collection of cached assets.
    /// </summary>
    private readonly Dictionary<string, object> _assets = new();

    #region Events

    private void Start()
    {
        if (AsyncStartup)
        {
            LoadStartupAssetsAsync();
        }
        else
        {
            LoadStartupAssets();
        }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Checks if the specified resource is cached.
    /// </summary>
    /// <param name="path">The path to the resource.</param>
    /// <returns>True if loaded, false if not.</returns>
    public bool ResourceLoaded(string path)
    {
        return _assets.ContainsKey(path);
    }

    /// <summary>
    /// Loads the desired resource synchronously.
    /// </summary>
    /// <typeparam name="T">The type of object to return.</typeparam>
    /// <returns>The loaded resource or a cached variant.</returns>
    /// <exception cref="Exception">The specified resource can't be found.</exception>
    public T LoadResource<T>(string path)
    {
        // Check for a cached variant.
        if (_assets.ContainsKey(path))
            // Return the cached object.
            return (T) _assets[path];

        // Load the resource.
        var resource = Resources.Load(path);
        // Validate the resource.
        if (resource == null)
            throw new Exception($"Unable to find resource {path}.");

        // Cache and return the resource.
        _assets.Add(path, resource);
        return (T) (object) resource;
    }

    /// <summary>
    /// Loads the desired resource asynchronously.
    /// </summary>
    /// <returns>The loaded resource or a cached variant.</returns>
    public async Task<T> LoadResourceAsync<T>(string path)
    {
        // Check for a cached variant.
        if (_assets.ContainsKey(path))
            // Return the cached object.
            return (T) _assets[path];

        // Load the resource.
        var request = (ResourceRequest) await Resources.LoadAsync(path);

        // Validate the resource.
        if (request.asset == null)
            throw new Exception($"Unable to find resource {path}.");

        // Cache and return the resource.
        _assets.Add(path, request.asset);
        return (T) (object) request.asset;
    }

    #endregion

    /// <summary>
    /// Loads the assets provided in startupAssets synchronously.
    /// </summary>
    private void LoadStartupAssets()
    {
        foreach (var asset in StartupLoad)
        {
            LoadResource<Object>(asset);
            Debug.Log($"Loaded resource {asset}.");
        }

        Debug.Log("Loaded all startup assets!");
    }

    /// <summary>
    /// Asynchronously loads the assets provided in startupAssets.
    /// </summary>
    private async void LoadStartupAssetsAsync()
    {
        foreach (var asset in StartupLoad)
            await LoadResourceAsync<Object>(asset);

        Debug.Log("Loaded all startup assets!");
    }
}