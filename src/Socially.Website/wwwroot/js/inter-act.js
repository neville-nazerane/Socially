﻿
window.setData = (key, value) => {
    localStorage.setItem(key, value);
};

window.getData = key => {
    return localStorage.getItem(key);
}