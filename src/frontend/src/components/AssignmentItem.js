function AssignmentItem({ assignment }) {
    return (
        <div className="assignment-card">
            <h4>{assignment.title}</h4>
            <p>{assignment.description}</p>
            <div className="assignment-meta">
                <span>Points: {assignment.points}</span>
                {assignment.githubLink && (
                    <a href={assignment.githubLink} target="_blank" rel="noopener noreferrer">
                        View on GitHub
                    </a>
                )}
            </div>
        </div>
    );
}

export default AssignmentItem;