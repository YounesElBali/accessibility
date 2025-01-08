// patterns.js
export const postalCodePattern = /^[0-9]{4}[a-zA-Z]{2}$/;

export const homeNumberPattern = /^[0-9]{4}[a-zA-Z]{1}$/;

export const streetNamePattern = /^[a-zA-Z]+$/;

export const usernamePattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])[A-Za-z\d!@#$%^&*]{7,}$/;

export const passwordPattern = /^(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*])(?!.*\b(\w+)\b)[A-Za-z\d!@#$%^&*]{7,}$/;


