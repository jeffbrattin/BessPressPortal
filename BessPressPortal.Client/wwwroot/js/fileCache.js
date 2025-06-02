window.fileCache = {
    async cacheFile(url) {
        const cache = await caches.open('offline-cache');
        await cache.add(url);
        return true;
    },

    async getCachedFile(url) {
        const cache = await caches.open('offline-cache');
        const response = await cache.match(url);
        if (!response) return null;
        return await response.text();
    }
};
