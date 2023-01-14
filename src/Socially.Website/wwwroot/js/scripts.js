
function init() {

    // theme switching
    delegate(document, 'click', '#darkModeSwitch', () => {
        let theme = localStorage.getItem('data-theme'); // Retrieve saved them from local storage
        if (theme === 'dark') {
            changeThemeToLight()
        } else {
            changeThemeToDark()
        }
    });

    // close nav on clicking item
    delegate(document, 'click', '.dropdown-item', () => {
        document.getElementsByClassName('dropdown-menu')[0]
                .classList
                .remove('show');

    });

    setupThemes();
    //e.fakePwd();
}

function setupThemes() {

    let theme = localStorage.getItem('data-theme');
    if (theme === 'dark') {
        changeThemeToDark()
    } else {
        changeThemeToLight();
    }
}

function changeThemeToDark() {

    var style = document.getElementById("style-switch");
    var dir = document.getElementsByTagName("html")[0].getAttribute('dir');

    document.documentElement.setAttribute("data-theme", "dark") // set theme to dark
    if (dir == 'rtl') {
        style.setAttribute('href', 'assets/css/style-dark-rtl.css');
    } else {
        style.setAttribute('href', 'css/social-theme/style-dark.css');
    }
    localStorage.setItem("data-theme", "dark") // save theme to local storage
}

function changeThemeToLight() {

    var style = document.getElementById("style-switch");
    var dir = document.getElementsByTagName("html")[0].getAttribute('dir');

    document.documentElement.setAttribute("data-theme", "light") // set theme light
    if (dir == 'rtl') {
        style.setAttribute('href', 'assets/css/style-rtl.css');
    } else {
        style.setAttribute('href', 'css/social-theme/style.css');
    }

    localStorage.setItem("data-theme", 'light') // save theme to local storage
}
