window.scrollBook = (page, height) => {
    const container = document.getElementById("book-container");
    if (container) {
        container.scrollTop = page * height;
    }
};