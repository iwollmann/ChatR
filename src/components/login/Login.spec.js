import React from 'react';
import { render } from 'enzyme';
import { expect } from 'chai';

import Login from './Login';

describe('<Login />', () => {
    it ('render the inputs', ()=>{
        const wrapper = render(<Login />);
        expect(wrapper.find('input').length).to.eq(2);
    });
})