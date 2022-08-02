export default {
  set(key, value) {
    try {
      if (typeof value === 'object') {
        window.localStorage.setItem(key, JSON.stringify(value));
      } else {
        window.localStorage.setItem(key, value);
      }
    } catch (e) {
      // console.error(e);
    }
  },
  get(key) {
    const value = window.localStorage.getItem(key);
    try {
      return JSON.parse(value);
    } catch (e) {
      return value;
    }
  },
  remove(key) {
    return window.localStorage.removeItem(key);
  }
}
;
