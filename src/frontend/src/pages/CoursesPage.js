import React, { useEffect, useState } from 'react';
import { getCourses } from '../api/api';
import CourseCard from '../components/CourseCard';
import './CoursesPage.css';

function CoursesPage() {
    const [courses, setCourses] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        async function loadCourses() {
            try {
                const data = await getCourses();
                setCourses(data);
            } catch (error) {
                console.error('Failed to load courses:', error);
            } finally {
                setLoading(false);
            }
        }

        loadCourses();
    }, []);

    if (loading) {
        return React.createElement(
            'div',
            { className: 'courses-loading' },
            'Загрузка курсов...'
        );
    }

    return React.createElement(
        'div',
        { className: 'courses-background' },
        [
            React.createElement('h2', { className: 'courses-title', key: 'title' }, 'Доступные курсы'),
            React.createElement(
                'div',
                { className: 'courses-list', key: 'list' },
                courses.map(course =>
                    React.createElement(CourseCard, { key: course.id, course })
                )
            )
        ]
    );
}

export default CoursesPage;
