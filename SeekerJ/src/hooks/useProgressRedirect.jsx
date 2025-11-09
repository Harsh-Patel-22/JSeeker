import { useEffect, useRef, useState } from 'react';

export const useProgressRedirect = (onComplete, speed = 200) => {
  const [progress, setProgress] = useState(0);
  const callbackRef = useRef(onComplete);

  // Always keep the latest onComplete
  useEffect(() => {
    callbackRef.current = onComplete;
  }, [onComplete]);

  useEffect(() => {
    console.log("useProgressRedirect triggered. speed:", speed);
    if (!speed) {
      setProgress(0);
      return;
    }

    let value = 0;
    const interval = setInterval(() => {
      value += 20;
      setProgress(value);
      console.log("Progress:", value);
      if (value >= 100) {
        clearInterval(interval);
        callbackRef.current?.();
      }
    }, speed);

    return () => clearInterval(interval);
  }, [speed]);

  return progress;
};
