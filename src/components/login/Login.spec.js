import React from 'react';
import { shallow, mount } from 'enzyme';
import { expect } from 'chai';

import Login from './Login';

describe('<Login />', () => {
    it ('render a <input /> component', ()=>{
        const wrapper = shallow(<Login />);
        expect(wrapper.contains(<input type="text" />)).to.eq(true);
    });
})