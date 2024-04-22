namespace FingroUssd.Interfaces;

public interface ICacheService
{
    /// <summary>
    /// Get Data using key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <returns></returns>
    T GetData<T>(string key);


    /// <summary>
    /// Checks if key exists in cache
    /// </summary>
    /// <returns>true or false</returns>
    bool HasData<T>(string key);

    /// <summary>
    /// Set Data with Value and Expiration Time of Key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    void SetData<T>(string key, T value);

    /// <summary>
    /// Remove Data
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    void RemoveData(string key);
}