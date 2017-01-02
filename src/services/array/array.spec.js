import { expect } from 'chai';
import { uniq } from './array';

describe('array', function () {
  it('distinct values in an array', function () {
    const array = ["item1", "item2", "item1"]

    expect(uniq(array).length).to.equal(2);
  });
});
