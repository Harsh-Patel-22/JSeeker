import { useState, useRef, useEffect } from 'react';

const CollapsibleSection = ({ title, children }) => {
  const [isOpen, setIsOpen] = useState(false);
  const contentRef = useRef();

  const handleToggle = () => {
    setIsOpen(prev => !prev);
  };

  useEffect(() => {
    if (isOpen && contentRef.current) {
      contentRef.current.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }, [isOpen]);

  return (
    <div className="mb-3">
      <button className="section-toggle d-flex justify-content-between align-items-center w-100" onClick={handleToggle}>
        <span>{title}</span>
        <span className={`chevron ${isOpen ? 'rotate' : ''}`}>&#9662;</span> {/* ▼ */}
      </button>
      <div
        className={`section-content ${isOpen ? 'open' : ''}`}
        ref={contentRef}
      >
        {children}
      </div>
    </div>
  );
}

export default CollapsibleSection;