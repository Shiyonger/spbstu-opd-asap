// Mock authentication service
export const loginWithGithub = () => {
    return new Promise((resolve) => {
        setTimeout(() => {
            console.log('Logged in with GitHub');
            resolve({ user: 'github_user' });
        }, 1000);
    });
};

export const logout = () => {
    return Promise.resolve();
};