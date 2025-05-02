import React from 'react'
import { Link } from 'react-router-dom';

function Navbar() {
    return (
        <header className="app-header">
            <nav>
                <Link to="/" className="logo">CoursesApp</Link>
                <div className="nav-links">
                    <Link to="/courses">All Courses</Link>
                </div>
            </nav>
        </header>
    );
}

export default Navbar;