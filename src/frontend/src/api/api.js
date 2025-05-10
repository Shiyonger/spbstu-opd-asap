// Mock data
const coursesData = [
    {
        id: 'is-oop-y25',
        name: 'Объектно-ориентированное программирование',
    },
    {
        id: 'web-dev-2023',
        name: 'Modern Web Development'
    },
    {   id: 'data-science-2023',
        name: 'Data Science Fundamentals'
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
                deadline: '2025-05-20T23:59:00Z',
                githubLink: 'https://github.com/oop-course/isu',
                googleSheetLink: "https://docs.google.com/spreadsheets/d/..."
            },
            {
                id: 'assignment-2',
                title: 'Shops System',
                description: 'E-commerce simulation',
                deadline: '2025-05-20T23:59:00Z',
                githubLink: 'https://github.com/oop-course/shops',
                googleSheetLink: "https://docs.google.com/spreadsheets/d/..."
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