export function set(key, val) {
    localStorage.setItem(key, val);
}

export function get(key) {
    return localStorage.getItem(key);
}

export function remove(key) {
    localStorage.removeItem(key);
}



