const LOGIN_API = "https://localhost:7058/api/login";
const SIGNUP_API = "https://localhost:7058/api/users";

function show(sectionId) {
    ["login-section", "signup-section", "app-section"].forEach(id => {
        const el = document.getElementById(id);
        if (el) el.style.display = "none";
    });
    const target = document.getElementById(sectionId);
    if (target) target.style.display = "block";
}

function login() {
    const username = document.getElementById("login-username").value;
    const password = document.getElementById("login-password").value;

    fetch(LOGIN_API, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ userName: username, password })
    })
        .then(async res => {
            if (!res.ok) throw new Error(await res.text());
            return res.text();
        })
        .then(token => {
            localStorage.setItem("jwt", token);
            document.getElementById("jwt-token").textContent = token;
            window.location.href = "index.html";
        })
        .catch(err => {
            document.getElementById("login-message").textContent = "Erreur: " + err.message;
        });
}

function signup() {
    const user = {
        username: document.getElementById("signup-username").value,
        password: document.getElementById("signup-password").value,
        emailAddress: document.getElementById("signup-email").value,
        surname: document.getElementById("signup-surname").value,
        givenName: document.getElementById("signup-givenname").value,
        role: document.getElementById("signup-role").value
    };

    fetch(SIGNUP_API, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(user)
    })
        .then(async res => {
            if (!res.ok) throw new Error(await res.text());
            document.getElementById("signup-message").textContent = "Inscription réussie. Connexion en cours...";

            document.getElementById("login-username").value = user.username;
            document.getElementById("login-password").value = user.password;

            login();
        })
        .catch(err => {
            document.getElementById("signup-message").textContent = "Erreur: " + err.message;
        });
}

function logout() {
    localStorage.removeItem("jwt");
    document.getElementById("login-username").value = "";
    document.getElementById("login-password").value = "";
    document.getElementById("jwt-token").textContent = "";
    document.getElementById("login-message").textContent = "";
    show("login-section");
}

if (localStorage.getItem("jwt")) {
    document.getElementById("jwt-token").textContent = localStorage.getItem("jwt");
    show("app-section");
} else {
    show("login-section");
}