import React from 'react';
import { Link } from 'react-router-dom';

function Navbar() {
    return React.createElement(
        'nav',
        { className: 'navbar' },
        React.createElement(
            'ul',
            { className: 'nav-list' },
            [
                React.createElement(
                    'li',
                    { key: 'home' },
                    React.createElement(
                        Link,
                        { to: '/' },
                        'Home'
                    )
                ),
                React.createElement(
                    'li',
                    { key: 'courses' },
                    React.createElement(
                        Link,
                        { to: '/courses' },
                        'Courses'
                    )
                )
            ]
        )
    );
}

export default Navbar;