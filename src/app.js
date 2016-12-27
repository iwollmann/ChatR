import React from 'react';
import { render } from 'react-dom';

const renderApp = () => {
    return (
      <div>
        <h1> Testing </h1>
      </div>
    );
  };

const App = () => renderApp();

render(<App />, document.getElementById('app')); // eslint-disable-line no-undef
