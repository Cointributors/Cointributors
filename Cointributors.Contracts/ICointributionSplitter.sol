// SPDX-License-Identifier: MIT
pragma solidity ^0.8.0;

interface ICointributionSplitter {
    function donate(uint256 amount, address tokenAddress) external;
}
