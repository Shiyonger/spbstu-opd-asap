import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import React from 'react';
import LoginPage from './pages/LoginPage';
import CoursesPage from './pages/CoursesPage';
import CourseDetailsPage from './pages/CourseDetailsPage';

function App() {
    return React.createElement(
        Router,
        null,
        React.createElement(
            'div',
            { className: 'App' },
            [
                React.createElement(
                    Routes,
                    null,
                    [
                        React.createElement(
                            Route,
                            {
                                path: "/",
                                element: React.createElement(LoginPage)
                            }
                        ),
                        React.createElement(
                            Route,
                            {
                                path: "/courses",
                                element: React.createElement(CoursesPage)
                            }
                        ),
                        React.createElement(
                            Route,
                            {
                                path: "/courses/:courseId",
                                element: React.createElement(CourseDetailsPage)
                            }
                        )
                    ]
                )
            ]
        )
    );
}

export default App;