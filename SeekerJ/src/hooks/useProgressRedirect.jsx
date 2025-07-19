import { useEffect, useState } from 'react';

export const useProgressRedirect = (onComplete, speed = 200) => {
  const [progress, setProgress] = useState(0);

  useEffect(() => {
    let value = 0;
    const interval = setInterval(() => {
      value += 20;
      setProgress(value);
      if (value >= 100) {
        clearInterval(interval);
        onComplete?.();
      }
    }, speed);
    return () => clearInterval(interval);
  }, [onComplete, speed]);

  return progress;
};
