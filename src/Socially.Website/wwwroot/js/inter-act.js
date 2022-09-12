
window.setData = (key, value) => {
    localStorage.setItem(key, value);
};

window.getData = key => {
    return localStorage.getItem(key);
}

window.removeData = key => {
    return localStorage.removeItem(key);
}

window.triggerClick = (elt) => elt.click();