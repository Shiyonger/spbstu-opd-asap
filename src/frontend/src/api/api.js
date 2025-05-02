// Mock data
const coursesData = [
    {
        id: 'is-oop-y25',
        name: 'Object-Oriented Programming',
        description: 'Learn OOP principles and design patterns',
        minPoints: 0,
        maxPoints: 100
    },
    {
        id: 'web-dev-2023',
        name: 'Modern Web Development',
        description: 'Full-stack development with React and Node.js',
        minPoints: 0,
        maxPoints: 120
    }
];

const courseDetails = {
    'is-oop-y25': {
        googleLink: 'https://docs.google.com/spreadsheets/d/oop-course',
        githubLink: 'https://github.com/orgs/oop-course',
        assignments: [
            {
                id: 'assignment-1',
                title: 'ISU',
                description: 'Individual student project',
                points: 4,
                githubLink: 'https://github.com/oop-course/isu'
            },
            {
                id: 'assignment-2',
                title: 'Shops System',
                description: 'E-commerce simulation',
                points: 8,
                githubLink: 'https://github.com/oop-course/shops'
            }
        ]
    }
};

// API functions
export const getCourses = async () => {
    return new Promise(resolve => {
        setTimeout(() => resolve(coursesData), 500);
    });
};

export const getCourseDetails = async (courseId) => {
    return new Promise((resolve, reject) => {
        setTimeout(() => {
            const course = courseDetails[courseId];
            if (course) {
                resolve({
                    ...coursesData.find(c => c.id === courseId),
                    ...course
                });
            } else {
                reject(new Error('Course not found'));
            }
        }, 500);
    });
};