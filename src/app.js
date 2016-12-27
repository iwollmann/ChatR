import React from 'react';
import { render } from 'react-dom';

import Routes from './components/routes';

const renderApp = () => {
    return (
      <div>
        <Routes />
      </div>
    );
  };

const App = () => renderApp();

render(<App />, document.getElementById('app')); // eslint-disable-line no-undef
